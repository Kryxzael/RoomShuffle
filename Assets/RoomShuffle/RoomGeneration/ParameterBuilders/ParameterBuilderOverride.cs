using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Provides parameter generation services for a limited number of rooms
/// </summary>
public abstract class ParameterBuilderOverride : ParameterBuilder
{
    /// <summary>
    /// Gets whether the parameter builder override will generate more rooms
    /// </summary>
    /// <returns></returns>
    public abstract bool HasNext();
}
