﻿using System;
using System.Collections.Generic;

namespace Lab3_ClassLibrary
{
    public class BinarySearchTree
    {
        private GameNode root;

        public GameNode Root => root;

        public BinarySearchTree(List<VideoGame> games)
        {
            games.Sort();

            root = Create(0, games.Count - 1, games);
        }

        private GameNode Create(int min, int max, List<VideoGame> games)
        {
            // return null if out of scope of the List
            if (max < min) return null;

            // Find the middle between the two indexes min and max
            int mid = (int)Math.Floor(((double)min + (double)max) / 2.0);

            // Create a node with the value at the mid index
            GameNode newNode = new GameNode(games[mid]);

            // Create the left reference
            newNode.left = Create(min, mid - 1, games);

            // Create the right reference
            newNode.right = Create(mid + 1, max, games);

            // Return the node that was created
            return newNode;
        }

        public string AllTheGames()
        {
            Queue<GameNode> queue = new Queue<GameNode>();

            string output = "";

            if (root == null) return $"There are no games in your list";

            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                // Pop the top node in the queue
                GameNode current = queue.Dequeue();

                // Attach the nodes information to the outputstring
                output += $"{current.game}";

                if (current.right != null) queue.Enqueue(current.right);
                if (current.left != null) queue.Enqueue(current.left);
            }

            return output;
        }

        public VideoGame Search(string title = null, string developer = null, string genre = null, Platform? platform = null, DateTime? releaseDate = null)
        {
            
            return Search(root, title, developer, genre, platform, releaseDate);
        }

        private VideoGame Search(GameNode node, string title, string developer, string genre, Platform? platform, DateTime? releaseDate)
        {
            if (node == null) return null;

            bool matches = true;

            // Check each condition
            if (!string.IsNullOrEmpty(title) && !node.game.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                matches = false;
            if (!string.IsNullOrEmpty(genre) && !node.game.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                matches = false;
            if (!string.IsNullOrEmpty(developer) && !node.game.Developer.Equals(developer, StringComparison.OrdinalIgnoreCase))
                matches = false;
            if (platform.HasValue && node.game.Platform != platform.Value)
                matches = false;
            if (releaseDate.HasValue && node.game.ReleaseDate.Date != releaseDate.Value.Date)
                matches = false;

            if (matches)
                return node.game;

            VideoGame left = Search(node.left, title, genre, developer, platform, releaseDate);

            if (left != null) 
                return left;
            
            return Search(node.right, title, genre, developer, platform, releaseDate);
        }
    }
}
