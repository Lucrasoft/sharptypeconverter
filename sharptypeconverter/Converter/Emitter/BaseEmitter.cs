

namespace Converter.Emitter
{
    internal abstract class BaseEmitter<TNodeType>
    {
        protected EmitterArguments EmitterArguments;
        protected BaseEmitter(EmitterArguments arguments)
        {
            EmitterArguments = arguments;    
        }
        internal abstract void Output(TNodeType arguments);
    }
}
