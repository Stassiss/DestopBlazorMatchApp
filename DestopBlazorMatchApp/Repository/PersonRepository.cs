using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DestopBlazorMatchApp.Models;
using Newtonsoft.Json;

namespace DestopBlazorMatchApp.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private const string filePath = @"wwwroot\Saves\person.json";
        private static string jsonString = File.ReadAllText(filePath);
        private static List<Person> PersonList = LoadSavedList();
        public List<Person> LoadPersonList(bool isMale)
        {
            return PersonList.Where(x => x.IsMale == isMale).ToList();
        }
        private static List<Person> LoadSavedList()
        {
            var list = new List<Person>();
            if (File.Exists(filePath))
            {
                list = JsonConvert.DeserializeObject<List<Person>>(jsonString);
            }
            return list;
        }
        private void SavePersonList()
        {
            try
            {
                var fromList = JsonConvert.SerializeObject(PersonList.ToArray());
                File.WriteAllText(filePath, fromList);
                var json = JsonConvert.SerializeObject(PersonList.ToArray());
                File.WriteAllText(filePath, json);
            }
            catch (Exception)
            {
                throw;

            }
        }
        public void AddPerson(Person person)
        {
            person.Id = GenerateId();
            PersonList.Add(person);
            SavePersonList();
        }
        private int GenerateId()
        {
            if (PersonList.Count < 1)
            {
                return 1;
            }
            var highestId = PersonList.Max(x => x.Id);
            return highestId + 1;
        }
        public void DeletePerson(Person person)
        {
            DeletePersonFromLikes(person);
            PersonList.Remove(person);
            SavePersonList();
        }
        private void DeletePersonFromLikes(Person selectedPerson)
        {
            foreach (var oppositeSexPerson in PersonList.Where(x => x.IsMale != selectedPerson.IsMale))
            {
                foreach (var like in oppositeSexPerson.Likes)
                {
                    if (like.LikedPersonId.Equals(selectedPerson.Id))
                    {
                        var likeToBeRemoved = oppositeSexPerson.Likes.Find(x => x.SelectedPersonId == like.SelectedPersonId
                                                                            && x.LikedPersonId == selectedPerson.Id);
                        oppositeSexPerson.Likes.Remove(likeToBeRemoved);
                        SavePersonList();
                        break;
                    }
                }

            }

        }

        public Person PersonInfo(int id)
        {
            return PersonList.Find(x => x.Id == id);
        }
        public void AddLikes(Person selcetedPerson, Person personToBeAddedToLikes)
        {


            var like = new Like();

            like.Id = selcetedPerson.Likes.Count < 1 ? 1 : selcetedPerson.Likes.Max(x => x.Id) + 1;
            like.SelectedPersonId = selcetedPerson.Id;
            like.LikedPersonId = personToBeAddedToLikes.Id;

            selcetedPerson.Likes.Add(like);
            SavePersonList();


        }
        public void RemoveLikes(Person selcetedPerson, Person personToBeRemovedFromLikes)
        {
            var likeToBeRemoved = selcetedPerson.Likes.Find(x => x.LikedPersonId == personToBeRemovedFromLikes.Id);


            selcetedPerson.Likes.Remove(likeToBeRemoved);
            SavePersonList();
        }
        public List<Person> GetOppositeSexList(Person selectedPerson)
        {
            var oppositeSexList = new List<Person>();
            if (selectedPerson.Likes.Count < 0)
            {
                return LoadPersonList(!selectedPerson.IsMale);
            }

            foreach (var person in LoadPersonList(!selectedPerson.IsMale))
            {
                if (!GetPersonListFromLikes(selectedPerson).Contains(person))
                {
                    oppositeSexList.Add(person);
                }
            }
            return oppositeSexList;
        }
        private List<Match> CheckForMatches()
        {
            List<Match> MatchList = new List<Match>();

            var maleList = LoadPersonList(true);
            foreach (var male in maleList)
            {
                foreach (var female in GetPersonListFromLikes(male))
                {
                    if (female.Likes.Count > 0)
                    {
                        foreach (var likedMale in GetPersonListFromLikes(female))
                        {
                            if (male.Id.Equals(likedMale.Id))
                            {
                                if (!MatchList.Any(x => x.MaleId == male.Id && x.FemaleId == female.Id))
                                {

                                    MatchList.Add(new Match() { MaleId = male.Id, FemaleId = female.Id });
                                    SavePersonList();
                                }

                            }
                        }
                    }

                }
            }
            return MatchList;
        }
        public List<Person> GetPersonListFromMatchesForInfoPage(Person selectedPerson)
        {
            var personList = new List<Person>();

            if (selectedPerson.IsMale)
            {
                var matchesForSelectedPerson = CheckForMatches().Where(x => x.MaleId == selectedPerson.Id);
                var oppositeSexMatches = matchesForSelectedPerson.Select(x => x.FemaleId);
                foreach (var femaleId in oppositeSexMatches)
                {
                    personList.Add(PersonInfo(femaleId));
                }
            }
            else if (true)
            {
                var matchesForSelectedPerson = CheckForMatches().Where(x => x.FemaleId == selectedPerson.Id);
                var oppositeSexMatches = matchesForSelectedPerson.Select(x => x.MaleId);
                foreach (var maleId in oppositeSexMatches)
                {
                    personList.Add(PersonInfo(maleId));
                }
            }

            return personList;
        }
        public List<Person> GetPersonListFromLikes(Person selectedPerson)
        {
            var personLikesList = new List<Person>();
            if (selectedPerson.Likes.Count < 1)
            {
                return personLikesList;
            };


            foreach (var like in selectedPerson.Likes)
            {
                var likedPerson = PersonInfo(like.LikedPersonId);
                personLikesList.Add(likedPerson);
            }

            return personLikesList;
        }

        public List<MatchDto> GetMatchDtoListFromMatches()
        {
            var matches = new List<MatchDto>();

            foreach (var match in CheckForMatches())
            {
                var matchDto = new MatchDto()
                {
                    Male = PersonInfo(match.MaleId),
                    Female = PersonInfo(match.FemaleId)
                };
                matches.Add(matchDto);
            }
            return matches;
        }

        public void ClearLists()
        {
            PersonList.Clear();
            SavePersonList();
        }
    }
}
