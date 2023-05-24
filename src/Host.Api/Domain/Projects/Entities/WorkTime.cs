using Host.Api.Core.Exceptions;
using Host.Api.Domain.Users.Entities;

namespace Host.Api.Domain.Projects.Entities
{
    public class WorkTime
    {
        public WorkTime(User user, Project project, DateTime startedAt, DateTime finishedAt)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            Project = project ?? throw new ArgumentNullException(nameof(project));
            StartedAt = startedAt;
            EndedAt = finishedAt;
        }

        public long UserId { get; private set; }
        public User User { get; private set; }
        public long ProjectId { get; private set; }
        public Project Project { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime EndedAt { get; set; }

        public void UpdateUser(User user)
        {
            if (user is null)
            {
                throw new DomainException("Usuário inválido.");
            }

            User = user;
        }

        public void UpdateProject(Project project)
        {
            if (project is null)
            {
                throw new DomainException("Projeto inválido.");
            }

            Project = project;
        }

        public void UpdateStartedAt(DateTime startedAt)
        {
            if (startedAt == default)
            {
                throw new DomainException("Data de início inválida.");
            }

            StartedAt = startedAt;
        }

        public void UpdateEndedAt(DateTime endedAt)
        {
            if (endedAt == default)
            {
                throw new DomainException("Data de fim inválida.");
            }

            EndedAt = endedAt;
        }
    }
}
