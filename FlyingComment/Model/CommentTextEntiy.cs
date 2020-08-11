using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyingComment.Model
{
    public class CommentTextEntiy
    {
        public string Comment { get; private set; }
        public CommentStyleEntity Style { get; private set; }

        public CommentTextEntiy( string comment, CommentStyleEntity style)
        {
            Comment = comment;
            Style = style;
        }

    }
}
