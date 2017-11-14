﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.MockBase;
using Wilhelm.Backend.Model;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Frontend.Model;

namespace Wilhelm.Frontend.Services.Interfaces
{
    public interface IHoldersService
    {
        void UpdateArchiveHolders(ICollection<ActivityHolder> activities, IEnumerable<ActivityDto> dtos);
        void UpateTodayActivityHolder(ICollection<ActivityHolder> activities, IEnumerable<ActivityDto> dtos);
        void UpdateActivityDtos(ICollection<ActivityDto> dtos, IEnumerable<ActivityHolder> activities);

        void UpdateConfigHolders(ICollection<GroupHolder> groups, ICollection<TaskHolder> tasks, ConfigDto config);
        void UpdateConfigDto(ConfigDto config, ICollection<GroupHolder> groups, ICollection<TaskHolder> tasks);

        int GenerateTemporaryId(IEnumerable<Holder> holders);
        string GetNameWithIndexIfNeeded(string startName, IEnumerable<NamedHolder> holders);

    }
}
