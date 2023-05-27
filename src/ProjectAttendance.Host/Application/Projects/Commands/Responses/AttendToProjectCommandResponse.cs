using ProjectAttendance.Core.Validators;
using System.Text.Json.Serialization;

namespace ProjectAttendance.Host.Application.Projects.Commands.Requests
{
    public class AttendToProjectCommandResponse : ICanBeValidated
    {
        [JsonPropertyName("time")]
        public AttendToProjectWorkTimeCommandResponse WorkTime { get; set; }
    }

    public class AttendToProjectWorkTimeCommandResponse
    {
        [JsonPropertyName("time_id")]
        public long WorkTimeId { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("project_id")]
        public long ProjectId { get; set; }

        [JsonPropertyName("started_at")]
        public DateTime StartedAt { get; set; }

        [JsonPropertyName("ended_at")]
        public DateTime EndedAt { get; set; }
    }
}