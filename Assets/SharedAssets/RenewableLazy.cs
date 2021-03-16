﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// A lazily initialized value that is refreshed if its value becomes null
/// </summary>
/// <typeparam name="T"></typeparam>
public class RenewableLazy<T>
{
    private T _value;

    /// <summary>
    /// Gets or creates the value of the renewable lazy
    /// </summary>
    public T Value
    {
        get
        {
            //Renews the value if necessary
            if ((ShouldRenew == null && _value == null) || (ShouldRenew != null && ShouldRenew()))
                Renew();

            return _value;
        }
    }

    /// <summary>
    /// The selector that will obtain a new value
    /// </summary>
    private Func<T> Selector { get; }

    /// <summary>
    /// The function that will determine if the value should be renewed. If this value is null, the value will only be renewed if it's null
    /// </summary>
    private Func<bool> ShouldRenew { get; }

    /// <summary>
    /// Creates a new RenewableLazy instance
    /// </summary>
    /// <param name="get"></param>
    public RenewableLazy(Func<T> get) : this(get, null)
    {  }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="get"></param>
    /// <param name="shouldRenew"></param>
    public RenewableLazy(Func<T> get, Func<bool> shouldRenew)
    {
        Selector = get;
        ShouldRenew = ShouldRenew;
    }

    /// <summary>
    /// Forces the value to be recalculated
    /// </summary>
    public T Renew()
    {
        _value = Selector();
        return _value;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Value.ToString();
    }
}
