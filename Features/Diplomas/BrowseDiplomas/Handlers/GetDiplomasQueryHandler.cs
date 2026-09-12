using exam_system.Common.Enums;
using exam_system.Domain.Entities.Diplomas;
using exam_system.Features.Diplomas.BrowseDiplomas.DTO;
using exam_system.Features.Diplomas.BrowseDiplomas.Queries;
using exam_system.Features.Shared;
using exam_system.Persistence.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace exam_system.Features.Diplomas.BrowseDiplomas.Handlers
{
    public class GetDiplomasQueryHandler : IRequestHandler<GetDiplomasQuery,RequestResponse<List<DiplomaItemsDTO>>>
    {
        private readonly IGenericRepository<Diploma> _diplomaRepository;

        public GetDiplomasQueryHandler(IGenericRepository<Diploma> diplomaRepository )
        {
            _diplomaRepository = diplomaRepository;
        }

        public async Task<RequestResponse<List<DiplomaItemsDTO>>> Handle(GetDiplomasQuery request, CancellationToken cancellationToken)
        {
            var diplomas = await _diplomaRepository.GetAll().Select
                (d=> new DiplomaItemsDTO
                {
                    Id = d.Id,
                    Title = d.Title,
                    Description = d.Description,
                    TotalQuizzes = d.Quizzes.Count(q=>q.Status == QuizStatus.Published),
                    CompletedQuizzes = d.Quizzes.SelectMany(q => q.Attempts)
                    .Count(a => a.StudentId == request.StudentId && (a.Status == AttemptStatus.InProgress || a.Status == AttemptStatus.TimedOut))


                }).ToListAsync(cancellationToken);
            return RequestResponse<List<DiplomaItemsDTO>>.Ok(diplomas);

        }
    }
}
