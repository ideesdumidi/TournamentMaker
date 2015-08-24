namespace TournamentMaker.BO.Tournaments
{
    public class PoolEliminationTournament:Tournament
    {
        public bool PoolFreeForAll { get; set; }
        public int QualificationSleeves { get; set; }
        public int PoolSleeves { get; set; }
        public int TeamsByMatch { get; set; }
    }
}
