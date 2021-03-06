﻿using System;
using System.Text;

namespace Blazor.Extensions.CssStyles.Css
{
    public class BrowserCssClassWriter : ICssClassWriter
    {
        private const int BuilderStartingCapacity = 200;

        private readonly ICssClassBuilder cssClassBuilder;
        private readonly ICssClassesCache cssClassesCache;
        private readonly ICssJsInterop cssJsInterop;

        public BrowserCssClassWriter(
            ICssClassBuilder cssClassBuilder,
            ICssClassesCache cssClassesCache,
            ICssJsInterop cssJsInterop)
        {
            this.cssClassBuilder = cssClassBuilder ?? throw new ArgumentNullException(nameof(cssClassBuilder));
            this.cssClassesCache = cssClassesCache ?? throw new ArgumentNullException(nameof(cssClassesCache));
            this.cssJsInterop = cssJsInterop ?? throw new ArgumentNullException(nameof(cssJsInterop));
        }

        public void WriteCssClass(ICssClass[] cssClasses)
        {
            var componentStyles = new StringBuilder(BuilderStartingCapacity);
            foreach (var cssClass in cssClasses)
            {
                cssClass.AssignUniqueName();
                if (cssClassesCache.CssClassIsAlreadyRealized(cssClass))
                {
                    continue;
                }

                var css = cssClassBuilder.BuildCssClassRepresentaion(cssClass);
                componentStyles.Append(css);

                cssClassesCache.StoreCssClassRealName(cssClass);
            }

            cssJsInterop.AppendCssStyleIntoDom(componentStyles.ToString());
        }
    }
}

