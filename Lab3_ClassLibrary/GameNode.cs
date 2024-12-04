using System;

namespace Lab3_ClassLibrary
{
    public class GameNode : IComparable<GameNode>
    {
        public VideoGame game;
        public GameNode left;
        public GameNode right;

        public GameNode(VideoGame game)
        {
            this.game = game;
            this.left = null;
            this.right = null;
        }

        public int CompareTo(GameNode other)
        {
            return this.game.CompareTo(other.game);
        }

        public override string ToString() => $"{game.ToString()}";
    }
}
