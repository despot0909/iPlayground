// SessionService.cs
using iPlayground.Core.Interfaces.Repositories;
using iPlayground.Core.Interfaces.Services;
using iPlayground.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iPlayground.Core.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IChildRepository _childRepository;
 

        public SessionService(
            ISessionRepository sessionRepository,
            IChildRepository childRepository)
        {
            _sessionRepository = sessionRepository;
            _childRepository = childRepository;

 
        }
        public async Task<Session> StartNewSessionAsync(Session session)
        {
            await _sessionRepository.AddAsync(session);
            await _sessionRepository.SaveChangesAsync();
            return session;
        }

        public async Task<Session> StartSessionAsync(int childId)
        {
            var child = await _childRepository.GetByIdAsync(childId);
            if (child == null)
                throw new Exception("Dijete nije pronađeno");

            var session = new Session
            {
                ChildId = childId,
                StartTime = DateTime.Now,
                IsFinished = false,
                IsSynced = false,
                InvoiceNumber = "N/N"
             };

            await _sessionRepository.AddAsync(session);
            await _sessionRepository.SaveChangesAsync();

            return session;
        }
        public async Task<Session> UpdateSessionAsync(Session session)
        {
            _sessionRepository.Update(session);
            await _sessionRepository.SaveChangesAsync();

            return session;

        }
        public async Task<Session> EndSessionAsync(int sessionId,decimal hr)
        {
            var session = await _sessionRepository.GetByIdAsync(sessionId);
            if (session == null)
                throw new Exception("Sesija nije pronađena");

            session.EndTime = DateTime.Now;
            session.IsFinished = true;
            session.TotalAmount = await CalculateSessionAmountAsync(sessionId,hr);

            _sessionRepository.Update(session);
            await _sessionRepository.SaveChangesAsync();

            return session;
        }

        public async Task<IEnumerable<Session>> GetActiveSessionsAsync()
        {
            return await _sessionRepository.GetActiveSessionsAsync();
        }

        public async Task<IEnumerable<Session>> GetInActiveSessionsAsync()
        {
            return await _sessionRepository.GetInActiveSessionsAsync();
        }



        public async Task<Session> GetSessionByIdAsync(int sessionId)
        {
            return await _sessionRepository.GetSessionWithChildAndParentAsync(sessionId);
        }

        public async Task<decimal> CalculateSessionAmountAsync(int sessionId, decimal hr)
        {
            var session = await _sessionRepository.GetByIdAsync(sessionId);
            if (session == null)
                throw new Exception("Sesija nije pronađena");

            var duration = (session.EndTime ?? DateTime.Now) - session.StartTime;
            var hours = Math.Ceiling(duration.TotalHours);
            decimal disc = session.TotalVaucer>0.00M ? session.TotalVaucer : 0.00M;

            // TODO: Učitati cijenu iz konfiguracije

            decimal hourlyRate = hr;
            decimal total = (decimal)hours * hourlyRate;

            return (decimal)(total-disc);
        }

        public async Task<bool> ConfirmSessionEndAsync(int sessionId)
        {
            var session = await _sessionRepository.GetByIdAsync(sessionId);
            if (session == null)
                throw new Exception("Sesija nije pronađena");

            if (!session.IsFinished)
                throw new Exception("Sesija nije završena");

            // TODO: Implementirati potvrdu od strane roditelja

            return true;
        }
 
    }
}