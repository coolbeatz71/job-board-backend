using Core.Application.CQRS;
using Job.Domain.Jobs.Dtos;
using Job.Domain.Jobs.Entities;
using Job.Domain.Jobs.Repository;
using Job.Domain.Jobs.ValueObjects;
using Mapster;

namespace Job.Application.UseCases.CreateJob;

public class CreateJobHandler(IJobRepository jobRepository) : ICommandHandler<CreateJobCommand, CreateJobResult>
{
    public async Task<CreateJobResult> Handle(CreateJobCommand command, CancellationToken cancellationToken)
    {
        var workMode = new WorkModeValue(command.WorkMode);
        var status = new JobStatusValue(command.Status);
        var jobType = new JobTypeValue(command.JobType);

        var job = JobEntity.Create(
            id: Guid.NewGuid(),
            title: command.Title,
            description: command.Description,
            requirements: command.Requirements,
            companyName: command.CompanyName,
            companyWebsite: command.CompanyWebsite,
            location: command.Location,
            workMode: workMode,
            status: status,
            jobType: jobType,
            applicationDeadline: command.ApplicationDeadline
        );

        var createdJob = await jobRepository.CreateAsync(job, cancellationToken);
        var jobDto = createdJob.Adapt<JobResponseDto>();

        return new CreateJobResult(jobDto);
    }
}