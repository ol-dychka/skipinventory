using System;
using Domain;

namespace Application.Interfaces;

public interface IJoinRequestRepository
{
    void Add(JoinRequest joinRequest);
}
