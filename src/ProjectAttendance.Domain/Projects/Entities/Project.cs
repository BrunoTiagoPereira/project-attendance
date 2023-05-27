using ProjectAttendance.Core.DomainObjects;
using ProjectAttendance.Core.Exceptions;
using ProjectAttendance.Domain.Users.Contracts;
using ProjectAttendance.Domain.Users.Entities;

namespace ProjectAttendance.Domain.Projects.Entities
{
    public class Project : AggregateRoot, IHasUsersRelated
    {
        public string Title { get; private set; }

        public string Description { get; private set; }

        private List<User> _users;
        public IReadOnlyCollection<User> Users => _users.AsReadOnly();

        private List<WorkTime> _workTimes;

        public IReadOnlyCollection<WorkTime> WorkTimes => _workTimes.AsReadOnly();

        protected Project() : base()
        {
            _users = new List<User>();
            _workTimes = new List<WorkTime>();
        }

        public Project(string title, string description, IEnumerable<User> users) : base()
        {
            UpdateTitle(title);
            UpdateDescription(description);
            UpdateUsers(users);
        }

        public void UpdateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new DomainException("Título inválido.");
            }

            Title = title;
        }

        public void UpdateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new DomainException("Descrição inválida.");
            }

            Title = description;
        }

        public void UpdateUsers(IEnumerable<User> users)
        {
            if (users is null || !users.Any())
            {
                throw new DomainException("Usuários inválidos.");
            }

            _users = users.ToList();
        }

        public WorkTime AttendTo(User user, DateTime startedAt, DateTime endedAt)
        {
            if(user is null)
            {
                throw new DomainException("Usuário inválido.");
            }

            if(!Users.Any(x => x.Id == user.Id))
            {
                throw new DomainException("Usuário não pertence ao projeto.");
            }

            if(startedAt > DateTime.Now)
            {
                throw new DomainException("A data de início deve ser menor que agora.");
            }

            if (startedAt > endedAt)
            {
                throw new DomainException("A data de início deve ser menor que a data fim.");
            }

            if (endedAt > DateTime.Now)
            {
                throw new DomainException("A data fim deve ser menor que agora.");
            }

            if (endedAt < startedAt)
            {
                throw new DomainException("A data fim deve ser maior que a data de início.");
            }

            var workTime = new WorkTime(user, this, startedAt, endedAt);
            _workTimes.Add(workTime);

            return workTime;
        }
    }
}