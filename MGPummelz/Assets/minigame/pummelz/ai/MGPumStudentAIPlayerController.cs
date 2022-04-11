using System;

namespace mg.pummelz
{
    public abstract class MGPumStudentAIPlayerController : MGPumAIPlayerController
    {
        public const string type = "STUDENT";

        //ONLY use this rng if you need random numbers
        internal Random rng;

        //return an array with your team member's Matrikel Numbers
        protected abstract int[] getTeamMartikels();
        //{
        //    return new int[]{1111111, 22222222};
        //}

        //you need to call this constructor
        //you also need to offer the player ID as a parameter in your constructor
        protected MGPumStudentAIPlayerController(int playerID) : base(playerID, true)
        {
            rng = new Random();
        }
    }
}
