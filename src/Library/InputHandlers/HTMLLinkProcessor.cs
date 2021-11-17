using System;
using Library.InputHandlers.Abstractions;
using Library.Utils;

namespace Library.InputHandlers
{
    /// <summary>
    /// This class represents a processor which generates a hypertext link.
    /// </summary>
    public class HTMLLinkProcessor : ProcessorWrapper<string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialResponseGetter"></param>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        public HTMLLinkProcessor(Func<string> initialResponseGetter) : base(
            PipeProcessor<string>.CreateInstance<string>(
                s => BasicUtils.IsValidHyperTextLink(s)
                    ? Result<string, string>.Ok(s)
                    : Result<string, string>.Err("The given link is invalid."),
                new BasicStringProcessor(initialResponseGetter))) {}
    }
}