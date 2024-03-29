﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model;
using Wilhelm.DataAccess;
using Wilhelm.Shared.Dto;

namespace Wilhelm.Backend.Services.Interfaces
{
    public interface IEntitiesService
    {
        void UpdateDto(ConfigDto dto, IEnumerable<WTask> wTasks, IEnumerable<WGroup> wGroups);
        void UpdateEntities(IDbSet<WTask> wTasks, IDbSet<WGroup> wGroups, ConfigDto config);
        void PrepareConfigToSave( IEnumerable<WTask> wTasks, IEnumerable<WGroup> wGroups);

        void UpdateDto(ICollection<ActivityDto> dtos, IEnumerable<WActivity> activities);
        void UpdateEntities(IDbSet<WActivity> activities, IEnumerable<ActivityDto> dtos);
        void UpdateEntity(IDbSet<WActivity> wActivities, ActivityDto activity);
    }
}
