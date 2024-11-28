using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_ClassLibrary
{
    internal class TreeNode : IComparable<TreeNode>
    {
        public VideoGame val;
        public TreeNode left;
        public TreeNode right;

        public TreeNode(VideoGame val)
        {
            this.val = val;
            this.left = null;
            this.right = null;
        }

        public int CompareTo(TreeNode other)
        {
            return this.val.CompareTo(other.val);
        }

        public override string ToString() => $"{val.ToString()}";
    }
}
