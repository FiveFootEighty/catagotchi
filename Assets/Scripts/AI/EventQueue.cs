using System;
using UnityEngine;
using Priority_Queue;

public class EventQueue
{
	private FastPriorityQueue<EventNode> priorityQueue;
	private LifeSpanQueue lifeSpanQueue;
	private const int MAX_CONCURRENT_EVENTS = 10;
	//private System.Timers.Timer ticker;

	public EventQueue(float updateInterval) {
		priorityQueue = new FastPriorityQueue<EventNode>(MAX_CONCURRENT_EVENTS);
		lifeSpanQueue = new LifeSpanQueue();
	}

	public void enqueue(AIEvent aiEvent) {
		EventNode node = new EventNode(aiEvent);
		priorityQueue.Enqueue(node, aiEvent.precedence);
		lifeSpanQueue.add(node, aiEvent.lifeSpan);
	}

	/**
	 * This needs to happen every second (or some time interval of our choosing)
	 */
	public void update() {
		// retrieve most recent node with lifespan 0
		EventNode node = lifeSpanQueue.rotate();

		// remove that node and all linked nodes from priorityQueue
		while (node != null) {
			priorityQueue.Remove(node);
			node = node.next;
		}
	}

	public AIEvent priority() {
		if (priorityQueue.Count == 0) {
			return null;
		}

		return priorityQueue.First.aiEvent;
	}



	/**
	 * 
	 */ 
	public class LifeSpanQueue {
		private const int MAX_EVENT_TIME = 5;
		private LifeSpanQueueNode head = new LifeSpanQueueNode();
		private LifeSpanQueueNode tail;

		public LifeSpanQueue() {
			head = new LifeSpanQueueNode();

			LifeSpanQueueNode node = new LifeSpanQueueNode();
			head.next = node;
			for (int i = 0; i < MAX_EVENT_TIME; i++) {
				node.next = new LifeSpanQueueNode();
				node = node.next;
			}
			tail = node;
		}

		public void add(EventNode eventNode, int lifeSpan) {
			LifeSpanQueueNode node = head;

			// iterate until we are at the correct node
			for (int i = 0; i < lifeSpan; i++) {
				node = node.next;
			}

			node.add(eventNode);
		}

		public EventNode rotate() {
			EventNode eventNode = head.mostRecentEvent;

			head.clear();
			tail.next = head;
			head = head.next;
			tail = tail.next;
			tail.next = null;

			return eventNode;
		}


		/**
		 * Node class for this LifeSpanQueue
		 */
		public class LifeSpanQueueNode {
			public LifeSpanQueueNode next;
			public EventNode mostRecentEvent;

			public void add(EventNode eventNode) {
				eventNode.next = mostRecentEvent;
				mostRecentEvent = eventNode;
			}

			public void clear() {
				mostRecentEvent = null;
			}
		}
	}


	/**
	 * 
	 */
	public class EventNode : FastPriorityQueueNode {

		public AIEvent aiEvent;

		public EventNode next;

		public EventNode(AIEvent aiEvent) {
			this.aiEvent = aiEvent;
		}
	}
}	