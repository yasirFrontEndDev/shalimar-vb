
Public Class CustomizeControl

    Public Shared Sub SetGridLayout(ByRef uwg As Infragistics.WebUI.UltraWebGrid.UltraWebGrid)
        With uwg
            .DisplayLayout.AllowColSizingDefault = Infragistics.WebUI.UltraWebGrid.AllowSizing.Free
            .DisplayLayout.RowSizingDefault = Infragistics.WebUI.UltraWebGrid.AllowSizing.Free
            .DisplayLayout.HeaderStyleDefault.Font.Bold = True
            .DisplayLayout.RowStyleDefault.Font.Size = 8
            .DisplayLayout.RowStyleDefault.Font.Bold = False

            .DisplayLayout.JavaScriptFileName = "script/ig_scripts/ig_WebGrid.js"
            .DisplayLayout.JavaScriptFileNameCommon = "script/ig_scripts/ig_shared.js"
            .ImageUrls.ImageDirectory = "images/ig_grid2_images"

            .DisplayLayout.HeaderStyleDefault.Height = Unit.Pixel(26)
            .DisplayLayout.HeaderStyleDefault.VerticalAlign = VerticalAlign.Middle
            .DisplayLayout.Pager.StyleMode = Infragistics.WebUI.UltraWebGrid.PagerStyleMode.ComboBox
            .DisplayLayout.Pager.PagerAppearance = Infragistics.WebUI.UltraWebGrid.PagerAppearance.Bottom

            .DisplayLayout.Pager.ComboStyle.HorizontalAlign = HorizontalAlign.Right
            .DisplayLayout.Pager.PagerStyle.Height = Unit.Pixel(20)
            .DisplayLayout.RowSelectorsDefault = Infragistics.WebUI.UltraWebGrid.RowSelectors.Yes
        End With
    End Sub

End Class