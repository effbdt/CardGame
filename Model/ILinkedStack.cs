using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface ILinkedStack<T>
    {
        void LinkeDStackOnTop(T card);
        T GetFromTop();
        void ShowCards(Action<T> show);
    }
}
