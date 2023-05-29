using ProjectAttendance.Core.DomainObjects;
using ProjectAttendance.Core.Exceptions;
using ProjectAttendance.Core.ValueObjects;
using ProjectAttendance.Domain.Projects.Entities;

namespace ProjectAttendance.Domain.Users.Entities
{
    public class User : AggregateRoot
    {
        public string Username { get; private set; }
        public string Login { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }

        private List<Project> _projects;
        public IReadOnlyCollection<Project> Projects => _projects.AsReadOnly();

        private List<WorkTime> _workTimes;
        public IReadOnlyCollection<WorkTime> WorkTimes => _workTimes.AsReadOnly();

        protected User()
        {
            _projects = new List<Project>();
            _workTimes = new List<WorkTime>();
        }

        public User(string username, string login, string email, string password) : this()
        {
            UpdateUsername(username);
            UpdateLogin(login);
            Email = new Email(email);
            Password = new Password(password);
        }

        public void UpdateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new DomainException("Nome inválido.");
            }

            Username = username;
        }

        public void UpdateLogin(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new DomainException("Login inválido.");
            }

            Login = login;
        }

        public void UpdateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new DomainException("Email inválido.");
            }

            Email = new Email(email);
        }

        public void UpdatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new DomainException("Senha inválida.");
            }

            Password = new Password(password);
        }
    }
}