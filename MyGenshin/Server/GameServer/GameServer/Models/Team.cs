﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;

using Common;
using Common.Data;

using Network;
using GameServer.Managers;
using GameServer.Entities;
using System.ServiceModel.Channels;
using GameServer.Services;

namespace GameServer.Models
{

    class FullTeamException : Exception
    {
        public override string Message => "队伍已满";
    }
    class NoMemberException : Exception
    {
        public override string Message => "没有这个成员";
    }

    class Team
    {
        public int ID;
        public int Count => Members.Count;
        const int Max = 4;
        public int TimeStamp =0;
        Character Leader;

        List<Character> Members = new List<Character>();


        public NTeamInfo GetInfo()
        {
            NTeamInfo info = new NTeamInfo()
            {
                Leader = Leader.Id,

            };
            info.Members.AddRange(Members.Select(x => x.Info));
            return info;
        }
        public Team ( Character leader)
        {
            AddMember(leader);
        }


        public void AddMember(Character member)
        {
            if (Count >= Max)
            {
                throw new FullTeamException();
            }
            else if (Count == 0)
            {
                Leader = member;
            }
            Members.Add(member);
            member.team = this;
            member.teamTimeStamp = TimeStamp;
            TimeStamp = Time.GetTimeStamp();
        }

        public void RemoveMember(Character member)
        {
            if (!Members.Remove(member))
            {
                throw new NoMemberException();
            }
            member.team = null;
            if (member == Leader)
            {
                if (Count > 0)
                {
                    Leader = Members[0];
                }
                else
                {
                    Leader = null;
                }
            }
            TimeStamp = Time.GetTimeStamp();
        }


    }
}
