﻿using FormContato.Context;
using FormContato.DTOs;
using FormContato.Models;

namespace FormContato.Repositories;

public class ContactRepository: Repository<ContactModel>, IContactRepository
{
    public ContactRepository(FCDbContext context) : base(context)
    {

    }
}
