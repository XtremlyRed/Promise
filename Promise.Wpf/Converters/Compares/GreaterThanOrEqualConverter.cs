﻿using System.Windows;

namespace Promise.Wpf;

/// <summary>
/// a class of <see cref="GreaterThanOrEqualConverter"/>
/// </summary>
/// <seealso cref="CompareConverter" />
public class GreaterThanOrEqualConverter : CompareConverter
{
    /// <summary>
    /// create a new instance of <see cref="GreaterThanOrEqualConverter"/>
    /// </summary>
    public GreaterThanOrEqualConverter()
        : base(CompareMode.GreaterThanOrEqual) { }
}
