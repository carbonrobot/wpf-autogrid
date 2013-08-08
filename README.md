wpf-autogrid
============

A flexible, easy to configure replacement for the standard WPF Grid control

AutoGrid lets you reduce the amount of xaml when using grids for layout by allowing you to define rows and columns as simple properties and alleviating you from having to explicitly specify the row and column a child control belongs to.

#### Standard WPF Grid

```
<Grid>
  <Grid.RowDefinitions>
    <RowDefinition Height="35" />
    <RowDefinition Height="35" />
  </Grid.RowDefinitions>
  <Grid.ColumnDefinitions>
    <ColumnDefinition Width="100" />
    <ColumnDefinition Width="auto" />
  </Grid.ColumnDefinitions>
  
  <Label Grid.Row="0" Grid.Column="0"/>
  <TextBox Grid.Row="0" Grid.Column="1"/>
  <Label Grid.Row="1" Grid.Column="0"/>
  <TextBox Grid.Row="1" Grid.Column="1"/>
</Grid>
```

#### AutoGrid (Same output as above)

```
<AutoGrid RowCount="2" RowHeight="35" Columns="100,auto">
  <Label />
  <TextBox />
  <Label />
  <TextBox />
</AutoGrid>
```

Notice how in the example above we didn't need to specify the row and column that each element belonged to; AutoGrid automatically figures out what row and column we wanted based on our configuration of the grid. AutoGrid uses a column first arrangement for its auto layout convention. 

Don't want AutoGrid to position elements automatically? **OK**

Explicit assignment of columns and rows still works too. This allows you to upgrade more easily. Most of time you can mix both without much trouble, but take care that this is not always the case.



