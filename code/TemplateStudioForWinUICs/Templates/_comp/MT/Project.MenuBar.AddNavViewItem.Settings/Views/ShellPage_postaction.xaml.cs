                </MenuBar>
<!--{[{-->
                <AppBarButton Grid.Column="1" x:Name="SettingsButton" AnimatedIcon.State="Normal"
                                                  PointerEntered="SettingsButton_PointerEntered"
                                                  PointerExited="SettingsButton_PointerExited"
                                                  Command="{x:Bind ViewModel.MenuSettingsCommand}">
                    <muxc:AnimatedIcon x:Name='AnimatedIcon'>
                        <muxc:AnimatedIcon.Source>
                            <animatedvisuals:AnimatedSettingsVisualSource/>
                        </muxc:AnimatedIcon.Source>
                        <muxc:AnimatedIcon.FallbackIconSource>
            <muxc:FontIconSource FontFamily="{StaticResource SymbolThemeFontFamily}" Glyph="&#xE713;"/>
                        </muxc:AnimatedIcon.FallbackIconSource>
                    </muxc:AnimatedIcon>
                </AppBarButton>
<!--}]}-->

