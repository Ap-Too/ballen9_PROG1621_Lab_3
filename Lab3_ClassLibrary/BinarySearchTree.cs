using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_ClassLibrary
{
    internal class BinarySearchTree
    {
        private TreeNode root;

        public TreeNode Root => root;

        public BinarySearchTree(List<> games)
        {
            games.Sort();

            root = Create(0, games.Count - 1, games);
        }

        private TreeNode Create(int min, int max, List<> games)
        {
            // return null if out of scope of the List
            if (max < min) return null;

            // Find the middle between the two indexes min and max
            int mid = (int)Math.Floor(((double)min + (double)max) / 2.0);

            // Create a node with the value at the mid index
            TreeNode newNode = new TreeNode(games[mid]);

            // Create the left reference
            newNode.left = Create(min, mid - 1, games);

            // Create the right reference
            newNode.right = Create(mid + 1, max, games);

            // Return the node that was created
            return newNode;
        }
    }
}
