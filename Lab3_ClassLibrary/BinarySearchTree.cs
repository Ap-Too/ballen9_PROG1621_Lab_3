using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Casting;

namespace Lab3_ClassLibrary
{
    internal class BinarySearchTree
    {
        private TreeNode root;

        public TreeNode Root => root;

        public BinarySearchTree(List<VideoGame> games)
        {
            games.Sort();

            root = Create(0, games.Count - 1, games);
        }

        private TreeNode Create(int min, int max, List<VideoGame> games)
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

        public string AllTheGames()
        {
            Queue<TreeNode> queue = new Queue<TreeNode>();

            string output = "LIST OF GAMES:\n\n";

            if (root == null) return $"There are no games in your list";

            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                // Pop the top node in the queue
                TreeNode current = queue.Dequeue();

                // Attach the nodes information to the outputstring
                output += $"{current.val.Title}" +
                    $"\n\tDeveloper: {current.val.Developer}" +
                    $"\n\tPlatform: {~current.val.Platform}" +
                    $"\n\tRelease Date: {current.val.ReleaseDate.ToShortDateString()}\n\n";

                if (current.right != null) queue.Enqueue(current.right);
                if (current.left != null) queue.Enqueue(current.left);
            }

            return output;
        }
    }
}
