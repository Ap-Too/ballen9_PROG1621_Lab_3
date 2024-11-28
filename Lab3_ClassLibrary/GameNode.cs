using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_ClassLibrary
{
    internal class GameNode : IComparable<GameNode>
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
