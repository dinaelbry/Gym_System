using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.TrainerViewModel;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repository.Classes;
using GymSystem.DAL.Repository.Interfaces;




namespace GymSystem.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
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

            return new TrainerViewModel
            {
                Id = trainer.Id,
                Photo = null,
                Name = trainer.Name,
                Email = trainer.Email,
                PhoneNumber = trainer.PhoneNumber,
                Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}",
                DateOfBirth = trainer.BirthDate.ToShortDateString(),
                Gender = trainer.Gender.ToString(),
                Specialization = trainer.Specialize.ToString()
            };

        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetById(trainerId, ct);
            if (trainer is null) return null;

            return new TrainerToUpdateViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.PhoneNumber,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                Specialties = trainer.Specialize
            };

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
            var trainer = new Trainer
            {
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                BirthDate = model.DateOfBirth,
                Gender = model.Gender,
                Specialize = model.Specialties,
                HireingDate = DateTime.Now,
                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City
                },
            };
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

            trainer.Email = model.Email;
            trainer.PhoneNumber = model.Phone;
            trainer.Specialize = model.Specialties;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Address.Street = model.Street;
            trainer.Address.City = model.City;
            trainer.UpdatedAt = DateTime.Now;

            repo.Update(trainer, ct);
            var result = await _unitOfWork.CompleteAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed to update trainer.");

        }

        public async Task<Result> DeleteTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var trainer = repo.GetById(trainerId, ct).Result;
            if (trainer is null) return Result.NotFound("Trainer not found.");

            var hasFutureSessions = await _unitOfWork.GetRepository<Session>()
                .Any(s => s.TrainerId == trainerId && s.StartDate > DateTime.Now, ct);
            if (hasFutureSessions)
                return Result.Fail("Cannot delete a trainer with upcoming sessions.");

            repo.Delete(trainer, ct);
            var result = await _unitOfWork.CompleteAsync(ct);

            return result > 0 ? Result.Ok() : Result.Fail("Failed to delete trainer.");
        }
    }
}
