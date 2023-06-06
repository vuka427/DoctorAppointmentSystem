﻿using DoctorAppointmentSystem.Models.Account;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace DoctorAppointmentSystem.App_Start
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterType<DbContext, DBContext>(TypeLifetime.Transient);
            container.RegisterType<ISystemParamService, SystemParamService>(TypeLifetime.Singleton);
            container.RegisterType<IMapper, MapperService>(TypeLifetime.Transient);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}