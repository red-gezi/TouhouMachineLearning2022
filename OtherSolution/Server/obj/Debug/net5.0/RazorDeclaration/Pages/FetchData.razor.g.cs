// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace Server.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\_Imports.razor"
using Server;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\_Imports.razor"
using Server.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\_Imports.razor"
using AntDesign;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\_Imports.razor"
using AntDesign.Charts;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\Pages\FetchData.razor"
using Server.Data;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/fetchdata")]
    public partial class FetchData : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 22 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\Pages\FetchData.razor"
       
	RenderFragment Card()
	{
		return
	

#line default
#line hidden
#nullable disable
        (__builder2) => {
            __builder2.OpenElement(0, "Template");
            __builder2.AddMarkupContent(1, "\r\n\t\t\t\t");
            __builder2.OpenElement(2, "AntDesign.Col");
            __builder2.AddAttribute(3, "Span", "6");
            __builder2.AddMarkupContent(4, "\r\n\t\t\t\t");
            __builder2.OpenElement(5, "Card");
            __builder2.AddAttribute(6, "Style", "width:300px;");
            __builder2.AddAttribute(7, "Bordered");
            __builder2.AddAttribute(8, "Cover", "coverTemplate");
            __builder2.AddMarkupContent(9, "\r\n\t\t\t\t");
            __builder2.OpenElement(10, "CardMeta");
            __builder2.AddAttribute(11, "AvatarTemplate", 
#nullable restore
#line 29 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\Pages\FetchData.razor"
                                           avatarTemplate

#line default
#line hidden
#nullable disable
            );
            __builder2.AddAttribute(12, "Title", "Meta Card");
            __builder2.AddAttribute(13, "Description", "This is the description");
            __builder2.CloseElement();
            __builder2.AddMarkupContent(14, "\r\n\t\t\t");
            __builder2.CloseElement();
            __builder2.AddMarkupContent(15, " \r\n\t\t\t");
            __builder2.CloseElement();
            __builder2.AddMarkupContent(16, "\r\n\t");
            __builder2.CloseElement();
        }
#nullable restore
#line 32 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\Pages\FetchData.razor"
               ;
	}
	RenderFragment avatarTemplate = 

#line default
#line hidden
#nullable disable
        (__builder2) => {
            __builder2.AddMarkupContent(17, "<Avatar src=\"https://zos.alipayobjects.com/rmsportal/ODTLcjxAfvqbxHnVXCYX.png\"></Avatar>");
        }
#nullable restore
#line 34 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\Pages\FetchData.razor"
                                                                                                                             ;

    RenderFragment coverTemplate = 

#line default
#line hidden
#nullable disable
        (__builder2) => {
            __builder2.AddMarkupContent(18, "<img alt=\"example\" src=\"https://gw.alipayobjects.com/zos/rmsportal/JiqGstEfoWAOHiTxclqi.png\">");
        }
#nullable restore
#line 36 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\Pages\FetchData.razor"
                                                                                                                                   ;

#line default
#line hidden
#nullable disable
#nullable restore
#line 69 "G:\UnityProject\TouhouMachineLearning2022\OtherSolution\Server\Pages\FetchData.razor"
       
	private WeatherForecast[]? forecasts;

	protected override async Task OnInitializedAsync()
	{
		forecasts = await ForecastService.GetForecastAsync(DateTime.Now);
	}

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private WeatherForecastService ForecastService { get; set; }
    }
}
#pragma warning restore 1591
