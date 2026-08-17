using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.TrainerViewModel;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repository.Interfaces;




namespace GymSystem.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper ;

        public TrainerService (IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
           var trainers =await _unitOfWork.GetRepository<Trainer>().GetAll(false,ct);
            return trainers.Select(t => _mapper.Map<TrainerViewModel>(t));  
             
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetById(trainerId, ct);
            if (trainer is null) return null;

            return _mapper.Map<TrainerViewModel>(trainer);

        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetById(trainerId, ct);
            if (trainer is null) return null;

            return _mapper.Map<TrainerToUpdateViewModel>(trainer);

        }

        public async Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            if (await repo.Any(t=> t.Email== model.Email, ct))
            {
                return Result.Fail("Trainer with this email already exists.");
            }
            if (await repo.Any(t => t.PhoneNumber == model.PhoneNumber, ct))
            {
                return Result.Fail("Trainer with this phone number already exists.");
            }
            var trainer = _mapper.Map<Trainer>(model);

          repo.Add(trainer, ct);
          var result = await _unitOfWork.CompleteAsync(ct);
          return result > 0 ? Result.Ok() : Result.Fail("Failed to create trainer.");
        }

        public async Task<Result> UpdateTrainerDetailsAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var trainer = await repo.GetById(trainerId, ct);
            if (trainer is null)  return Result.NotFound("Trainer not found.");
            
            if (await repo.Any(t => t.Email == model.Email && t.Id != trainerId, ct))
                return Result.Fail("Another trainer is already using this email.");
            if (await repo.Any(t => t.PhoneNumber == model.Phone && t.Id != trainerId, ct))
                return Result.Fail("Another trainer is already using this phone number.");

            _mapper.Map(model, trainer);

            repo.Update(trainer, ct);
            var result = await _unitOfWork.CompleteAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed to update trainer.");

        }

        public async Task<Result> DeleteTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var trainer = repo.GetById(trainerId, ct).Result;
            if (trainer is null) return Result.NotFound("Trainer not found.");

       
            if (await HasFutureSessionsAsync(trainerId, ct))
                return Result.Fail("Cannot delete a trainer with upcoming sessions.");

            repo.Delete(trainer, ct);
            var result = await _unitOfWork.CompleteAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to delete trainer.");
        }



        // Private helper method to check if a trainer has future sessions 
        private async Task<bool> HasFutureSessionsAsync(int trainerId, CancellationToken ct)
        {
            return await _unitOfWork.GetRepository<Session>()
                .Any(s => s.TrainerId == trainerId && s.StartDate > DateTime.Now, ct);
        }
    }
}
