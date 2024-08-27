
# Common Attribute Order in XAML

## <u> Key Properties (Name, x):</u>

Always start with the element's Name or x:Name if it has one, as this is typically the most important identifier for an element.

## <u> Layout Properties (Grid properties, DockPanel properties, etc.):</u>

**These are properties that define the element's position and size within a parent container.**

*Examples include: Grid.Row, Grid.Column, DockPanel.Dock,<br>
Width, Height, MinWidth, MinHeight, MaxWidth, MaxHeight, Margin, Padding*

## <u> Content Properties:</u>

**These define what is displayed or contained within the element.**
*For example, Content for a Button, Text for a TextBlock or TextBox, etc.*

## <u> Behavioral Properties:</u>

**These include properties that affect the behavior of the control.**
*Examples: IsEnabled, IsReadOnly, IsChecked, IsHitTestVisible.*

## <u> Appearance Properties:</u>

**Properties that affect the visual styling of the element.**
*Examples: Background, Foreground, FontSize, FontFamily, BorderBrush, BorderThickness, etc.*

## <u> Events:</u>

**Event handlers for user interactions.**

*Examples: Click, MouseEnter, MouseLeave, Loaded, etc.*

## <u> Miscellaneous Properties:</u>

**Any other properties that don’t fit into the above categories.**

## <u> Attached Properties:</u>

**Properties that belong to a different element but are attached to the current element.**
*Examples: Canvas.Left, Canvas.Top, ScrollViewer.CanContentScroll, etc.**



<Button x:Name="NavigateButton"              <!-- Key Property -->
        Grid.Row="1" Grid.Column="0"         <!-- Layout Properties -->
        Width="100" Height="30" Margin="5"   <!-- Layout Properties -->
        Content="&lt;"                       <!-- Content Property -->
        IsEnabled="True"                     <!-- Behavioral Property -->
        Background="LightGray"               <!-- Appearance Properties -->
        Click="NavigateButton_Click" />      <!-- Event Handler -->
