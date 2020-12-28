using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DestopBlazorMatchApp.Models;

namespace DestopBlazorMatchApp.Repository
{
    public interface IPersonRepository
    {
        void AddLikes(Person selcetedPerson, Person personToBeAddedToLikes);
        void AddPerson(Person person);
        void DeletePerson(Person person);
        List<Person> GetOppositeSexList(Person selectedPerson);
        List<Person> GetPersonListFromLikes(Person selectedPerson);
        List<MatchDto> GetMatchDtoListFromMatches();

        /// <summary>
        /// Load {isMale} person list
        /// </summary>
        /// <param name="isMale"></param>
        /// <returns>Male or Female list</returns>
        List<Person> LoadPersonList(bool isMale);
        Person PersonInfo(int id);
        void RemoveLikes(Person selcetedPerson, Person personToBeAddedToLikes);
        List<Person> GetPersonListFromMatchesForInfoPage(Person selectedPerson);
    }
}
