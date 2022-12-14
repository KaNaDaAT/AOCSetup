using System;
using System.Collections.Generic;

namespace AdventOfCoding.Lib {
	public class AOCNode {
		public int X;
		public int Y;
		public int Cost;
		public int Distance = int.MaxValue;
		public bool Visited = false;
		(int, int)[] adjacent = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

		public AOCNode(int x, int y, int cost) {
			this.X = x;
			this.Y = y;
			this.Cost = cost;
			this.Distance = int.MaxValue;
		}

		public AOCNode(AOCNode node) {
			this.X = node.X;
			this.Y = node.Y;
			this.Cost = node.Cost;
			this.Distance = int.MaxValue;
		}

		public IEnumerable<AOCNode> GetAdjacent(Dictionary<(int x, int y), AOCNode> graph, int rowSize, int columnSize) {
			foreach((int i, int j) in adjacent) {
				(int x, int y) key = (this.X + i, this.Y + j);
				if(key.x >= 0 && key.x < rowSize && key.y >= 0 && key.y < columnSize && !graph[key].Visited) {
					yield return graph[key];
				}
			}
		}
	}
}