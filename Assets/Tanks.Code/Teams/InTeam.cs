namespace Tanks.Teams {
    using System;
    using Scellecs.Morpeh;

    [Serializable]
    public struct InTeam : IComponent 
    {
        public Entity team;
    }

    public static class TeamExtensions 
    {
        public static bool InSameTeam(this Entity first, Entity second) 
        {
            var world = first.GetWorld();
            var stash = world.GetStash<InTeam>();

            InTeam firstInTeam = stash.Get(first, out bool firstHasTeam);
            InTeam secondInTeam = stash.Get(second, out bool secondHasTeam);
            if (!firstHasTeam || !secondHasTeam) {
                return false;
            }

            return firstInTeam.team.Equals(secondInTeam.team);
        }
    }
}