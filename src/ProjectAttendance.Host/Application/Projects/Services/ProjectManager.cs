using Microsoft.EntityFrameworkCore;
using ProjectAttendance.Application.Users.Services;
using ProjectAttendance.Core.Exceptions;
using ProjectAttendance.Core.Transaction;
using ProjectAttendance.Core.Validators;
using ProjectAttendance.Domain.Projects.Entities;
using ProjectAttendance.Domain.Projects.Repositories;
using ProjectAttendance.Domain.Users.Repositories;
using ProjectAttendance.Host.Application.Projects.Commands.Requests;
using ProjectAttendance.Host.Application.Projects.Queries.Requests;
using ProjectAttendance.Host.Application.Projects.Queries.Responses;

namespace ProjectAttendance.Host.Application.Projects.Services
{
    public class ProjectManager : IProjectManager
    {
        private readonly IValidatorManager _validatorManager;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserAccessorManager _userAccessorManager;
        private readonly IUnitOfWork _uow;

        public ProjectManager(IValidatorManager validatorManager, IUserRepository userRepository, IProjectRepository projectRepository, IUserAccessorManager userAccessorManager, IUnitOfWork uow)
        {
            _validatorManager = validatorManager ?? throw new ArgumentNullException(nameof(validatorManager));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _userAccessorManager = userAccessorManager ?? throw new ArgumentNullException(nameof(userAccessorManager));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public async Task<AttendToProjectCommandResponse> AttendToProjectAsync(AttendToProjectCommandRequest request)
        {
            _validatorManager.ThrowIfInvalid(request);

            var currentUserId = _userAccessorManager.GetCurrentUserId();

            var user = await _userRepository.FindAsync(request.UserId);

            if (request.UserId != currentUserId)
            {
                throw new DomainException("Não é possível fazer apontamentos de horas de outro usuário.");
            }

            if (user is null)
            {
                throw new DomainException("Usuário não existe.");
            }

            var project = await _projectRepository.GetProjectWithUsersAndWorkTimesAsync(request.ProjectId);

            if (project is null)
            {
                throw new DomainException("Projeto não existe.");
            }

            if (!project.Users.Any(x => x.Id == request.UserId))
            {
                throw new DomainException("O usuário não pertence ao projeto.");
            }

            var workTime = project.AttendTo(user, request.StartedAt, request.EndedAt);

            _projectRepository.Update(project);
            await _uow.CommitAsync();

            return new AttendToProjectCommandResponse
            {
                WorkTime = new AttendToProjectWorkTimeCommandResponse
                {
                    WorkTimeId = workTime.Id,
                    UserId = request.UserId,
                    EndedAt = request.EndedAt,
                    ProjectId = request.ProjectId,
                    StartedAt = request.StartedAt,
                }
            };
        }

        public async Task<CreateProjectCommandResponse> CreateProjectAsync(CreateProjectCommandRequest request)
        {
            _validatorManager.ThrowIfInvalid(request);

            var currentUserId = _userAccessorManager.GetCurrentUserId();
            var requestUsers = request.UsersIds ?? new List<long>();

            if (!requestUsers.Any(x => x == currentUserId))
            {
                requestUsers.Add(currentUserId);
            }

            var users = await _userRepository.Set.Where(x => requestUsers.Contains(x.Id)).ToArrayAsync();

            if (requestUsers.Distinct().Count() != users.Length)
            {
                throw new DomainException("Algum usuário não existe.");
            }

            var project = new Project(request.Title, request.Description, users);

            await _projectRepository.AddAsync(project);
            await _uow.CommitAsync();

            return new CreateProjectCommandResponse
            {
                Project = new CreateProjectProjectCommandResponse
                {
                    ProjectId = project.Id,
                    Title = request.Title,
                    Description = request.Description,
                    UsersIds = requestUsers
                }
            };
        }

        public async Task<GetProjectQueryResponse> GetProjectAsync(GetProjectQueryRequest request)
        {
            _validatorManager.ThrowIfInvalid(request);

            var project = await _projectRepository.GetProjectWithUsersAndWorkTimesAsync(request.ProjectId);

            if (project is null)
            {
                throw new DomainException("Projeto não encontrado");
            }

            _userAccessorManager.ThrowIfUserDontHasAccess(project);

            return new GetProjectQueryResponse
            {
                Project = new GetProjectProjectQueryResponse
                {
                    Id = project.Id,
                    Title = project.Title,
                    Description = project.Description,
                    Users = project.Users.Select(x => new GetProjectProjectUserQueryResponse { Id = x.Id, Username = x.Username }).ToList(),
                    WorkTimes = project.WorkTimes.Select(x => new GetProjectProjectWorkTimeQueryResponse { Id = x.Id, UserId = x.UserId, Username = x.User.Username, StartedAt = x.StartedAt, EndedAt = x.EndedAt }).ToList()
                }
            };
        }
    }
}