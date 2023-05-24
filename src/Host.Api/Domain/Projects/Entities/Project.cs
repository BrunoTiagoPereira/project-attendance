using Host.Api.Core.Exceptions;
using Host.Api.Domain.Users.Entities;

namespace Host.Api.Domain.Projects.Entities
{
    public class Project
    {
        public string Title { get; private set; }

        public string Description { get; private set; }

        private List<User> _users;
        public IReadOnlyCollection<User> Users => _users.AsReadOnly();

        private List<WorkTime> _workTimes;

        public IReadOnlyCollection<WorkTime> WorkTimes => _workTimes.AsReadOnly();

        protected Project()
        {
            _users = new List<User>();
            _workTimes = new List<WorkTime>();
        }

        public Project(string title, string description, IEnumerable<User> users)
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
    }
}
