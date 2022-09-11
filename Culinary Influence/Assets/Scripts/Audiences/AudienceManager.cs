using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using UI;
using UnityEngine;
using UnityEngine.Assertions;
using Utility;

namespace Audiences
{
    public class AudienceManager : MonoBehaviour
    {
        public AudienceMember[] members;
        public Transform[] spawnPoints;

        [SerializeField] private GameOverUI gameOver;

        private Dictionary<ScriptableEnum, AudienceMember> _membersMap;

        private void Awake()
        {
            EndOfMapController.OnPlayerLost += OnMemberLost;

            _membersMap = new Dictionary<ScriptableEnum, AudienceMember>();

            Assert.AreEqual(members.Length, spawnPoints.Length);

            members.Shuffle();

            for (var i = 0; i < members.Length; i++)
            {
                AudienceMember member = members[i];
                Transform spawnPoint = spawnPoints[i];

                AudienceMember inst = Instantiate(member);
                inst.transform.position = spawnPoint.position;

                _membersMap.Add(inst.MemberIdentifier, inst);
            }
        }

        private void OnDestroy()
        {
            EndOfMapController.OnPlayerLost -= OnMemberLost;
        }

        private void OnMemberLost(ScriptableEnum member, PreferredParty party)
        {
            if (!_membersMap.TryGetValue(member, out AudienceMember foundMember))
            {
                return;
            }

            PreferredParty oppositeParty = party switch
            {
                PreferredParty.Red => PreferredParty.Blue,
                PreferredParty.Blue => PreferredParty.Red,
                _ => throw new ArgumentOutOfRangeException(nameof(party), party, null)
            };

            foundMember.SetParty(oppositeParty);
            foundMember.gameObject.SetActive(true);

            int redCount = _membersMap.Count(x => x.Value.Party == PreferredParty.Red);
            int blueCount = _membersMap.Count(x => x.Value.Party == PreferredParty.Blue);

            if (redCount >= 2)
            {
                gameOver.ShowGameOverScreen(PreferredParty.Red);
            }
            else if (blueCount >= 2)
            {
                gameOver.ShowGameOverScreen(PreferredParty.Blue);
            }
        }

        public AudienceMember GetNonDecidedMember()
        {
            KeyValuePair<ScriptableEnum, AudienceMember> member = _membersMap
                .FirstOrDefault(m =>
                    m.Value.HasNoParty
                    && m.Value.gameObject.activeSelf
                );

            return member.Value;
        }
    }
}