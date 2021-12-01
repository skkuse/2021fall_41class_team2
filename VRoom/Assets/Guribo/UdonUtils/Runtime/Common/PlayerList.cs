using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Guribo.UdonUtils.Runtime.Common
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class PlayerList : UdonSharpBehaviour
    {
        public int[] players = new int[0];

        #region Mandatory references

        [Header("Mandatory references")]
        public UdonDebug udonDebug;

        #endregion

        #region public
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <returns>true if player was added and false if already in the list</returns>
        public bool AddPlayer(VRCPlayerApi player)
        {
            var validPlayers = ConsolidatePlayerIds(players);
            
            if (!udonDebug.Assert(Utilities.IsValid(player), "Player invalid", this))
            {
                ResizePlayerArray(validPlayers);
                return false;
            }

            if (players == null || players.Length == 0)
            {
                players = new[]
                {
                    player.playerId
                };
                return true;
            }
            
            var playerNotInList = Array.BinarySearch(players, player.playerId) < 0;
            if (!udonDebug.Assert(playerNotInList, $"Player {player.playerId} already in list", this))
            {
                ResizePlayerArray(validPlayers);
                return false;
            }

            var tempArray = new int[validPlayers + 1];
            tempArray[0] = player.playerId;
            Array.ConstrainedCopy(players,0,tempArray,1, validPlayers);
            players = tempArray;
            Sort(players);
            return true;
        }
        
        public bool RemovePlayer(VRCPlayerApi player)
        {
            var count = ConsolidatePlayerIds(players);
            
            if (!Utilities.IsValid(player)
                || DiscardInvalid() == 0)
            {
                ResizePlayerArray(count);
                return false;
            }

            var playerIndex = Array.BinarySearch(players, player.playerId);
            if (playerIndex < 0)
            {
                ResizePlayerArray(count);
                return false;
            }

            var tempArray = new int[count - 1];

            if (playerIndex > 0)
            {
                Array.ConstrainedCopy(players, 0, tempArray, 0, playerIndex);
            }

            if (tempArray.Length - playerIndex > 0)
            {
                Array.ConstrainedCopy(players, playerIndex + 1, tempArray, playerIndex, tempArray.Length - playerIndex);
            }

            players = tempArray;
            return true;
        }

        public bool Contains(VRCPlayerApi playerApi)
        {
            if (!Utilities.IsValid(playerApi)
                || DiscardInvalid() == 0)
            {
                return false;
            }

            return Array.BinarySearch(players, playerApi.playerId) > -1;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>number of valid players in the list after disposing of all invalid ids</returns>
        public int DiscardInvalid()
        {
            var validPlayers = ConsolidatePlayerIds(players);
            ResizePlayerArray(validPlayers);
            return validPlayers;
        }

        public void Clear()
        {
            players = new int[0];
        }
        
        #endregion

        #region Internal

        internal bool ResizePlayerArray(int length)
        {
            if (length < 0)
            {
                return false;
            }
            if (players == null || players.Length == 0)
            {
                players = new int[length];
                for (var i = 0; i < players.Length; i++)
                {
                    players[i] = Int32.MaxValue;
                }
                return true;
            }

            var copyLength = length > players.Length ? players.Length : length;
            var temp = new int[length];
            Array.ConstrainedCopy(players, 0, temp, 0, copyLength);
            players = temp;
            for (var i = copyLength; i < players.Length; i++)
            {
                players[i] = Int32.MaxValue;
            }
            return true;
        }
        
        internal void Sort(int[] array)
        {
            if (array == null || array.Length < 2)
            {
                return;
            }

            BubbleSort(array);
        }

        /// TODO this is bad an must be replaced at some point!!!
        internal void BubbleSort(int[] array)
        {
            var arrayLength = array.Length;
            for (var i = 0; i < arrayLength; i++)
            {
                for (var j = 0; j < arrayLength - 1; j++)
                {
                    var next = j + 1;

                    if (array[j] > array[next])
                    {
                        var tmp = array[j];
                        array[j] = array[next];
                        array[next] = tmp;
                    }
                }
            }
        }

        internal int ConsolidatePlayerIds(int[] list)
        {
            if (list == null)
            {
                return 0;
            }

            var valid = 0;
            var moveIndex = -1;
            for (var i = 0; i < list.Length; i++)
            {
                if (Utilities.IsValid(VRCPlayerApi.GetPlayerById(list[i])))
                {
                    ++valid;
                    if (moveIndex != -1)
                    {
                        list[moveIndex] = list[i];
                        list[i] = int.MaxValue;
                        ++moveIndex;
                    }
                }
                else
                {
                    // ensure that the entry no longer references a valid player
                    list[i] = int.MaxValue;
                    if (moveIndex == -1)
                    {
                        moveIndex = i;
                    }
                }
            }

            return valid;
        }
        
        #endregion
    }
}