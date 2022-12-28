using ToDo_List_with_additions.Models;

namespace ToDo_List_with_additions.Services
{
    public interface IStatisticsService
    {
        public StatisticsModel Create(string userId);
        public StatisticsModel IncrementDone(string userId, int importance);
        public StatisticsModel IncrementNotDone(string userId, int importance);
        public StatisticsModel IncrementPostponed(string userId, int importance);
        public StatisticsModel DecrementNotDone(string userId, int importance);
    }
}
