using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class EmptyDeckException() : Exception
	{
	}

	public class Node<T>
	{
		public T card;
		public Node<T> next;

		public Node(T card, Node<T> nextCard)
		{
			this.card = card;
			this.next = nextCard;
		}
	}
	public class LinkedStack<T> : ILinkedStack<T>
	{
		Node<T> head;
		public int n;

		public int Length
		{
			get
			{ return n; }
		}

		public LinkedStack()
		{
			head = null;
			n = -1;
		}

		public bool IsEmpty()
		{
			return n < 0;
		}

		public void LinkeDStackOnTop(T card)
		{
			Node<T> newNode = new Node<T>(card, head);
			head = newNode;
			n++;
		}

		public T GetFromTop()
		{
			if (n > -1)
			{
				T card = head.card;
				head = head.next;
				n--;
				return card;
			}
			else throw new EmptyDeckException();
		}

		public void ShowCards(Action<T> show)
		{
			Node<T> p = head;
			while (p != null)
			{
				show(head.card);
				p = p.next;
			}
		}
	}


}
