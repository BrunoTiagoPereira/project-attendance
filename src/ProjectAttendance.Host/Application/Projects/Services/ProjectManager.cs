﻿using ProjectAttendance.Application.Users.Services;
using ProjectAttendance.Core.Exceptions;
using ProjectAttendance.Core.Transaction;
using ProjectAttendance.Core.Validators;
using ProjectAttendance.Domain.Projects.Repositories;
using ProjectAttendance.Domain.Users.Repositories;
using ProjectAttendance.Host.Application.Projects.Commands.Requests;

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

        public async Task<AttendToProjectCommandResponse> AttendToProject(AttendToProjectCommandRequest request)
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

            if(project is null)
            {
                throw new DomainException("Projeto não existe.");
            }

            if(!project.Users.Any(x => x.Id== request.UserId))
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
    }
}