   M S B A M L   `   `   `  ���� H  DSteganography, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:SteganographySteganography  H DMahApps.Metro, Version=1.1.3.5, Culture=neutral, PublicKeyToken=nullV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls     Steganography.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     1	converterclr-namespace:Steganography     PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     "   Steganography.WindowBehavior    HideCloseButton  .  � Title$ Steganography��$	��550��$	��625�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=� � WindowStyle$ 
ToolWindow=�(  "MahApps.Metro.Controls.MetroWindow   ShowCloseButton .  #��� ��   EnableDWMDropShadow .     NonActiveWindowTitleBrush# � ��
�������   WindowTitleBrush �	#FF1C9362$	��550��$	��550��$	��625��$	��625��	 � 	Resources	 +  %Steganography.BoolVisibilityConverter   BoolVisibilityConverter��%   &	          .��� 
 � RowDefinitions
 �� $��* ��� $	��10* ��� $��* ��� $
��3.5* ��� $��* � � ColumnDefinitions �� $��2* ��� $��* �.+�o� $��0��$��2��$��0��$
��Auto��
�������$��2q����� .R�]� $��Stretch=�.@�m� $��Image��$��10,0,0,0q�.@�m� -   $
��Open���� m� -   $
��Save��m� -   $��About��m� -   $��Close��$��Right=�$��0,0,80,2q�
���  ���� $��0��$��0��$��2��$��Bottom=��� $��1��$��1��
�������$��2q�$��2��$
��Left=�$��10,-3,0,-173q� � LayoutTransform ��  �� Angle$ 90Q��� $��4��$��1��$��Łukasz Paczyński��$��Center=�$��Center=�$��20(�$��Buxton Sketch)��� $��1��$��0��$��1q����	#FF000000$	��374��$	��290��$	��Top=�$��Right=�$��	0,20,20,0q�
������� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     
BlurRadius$ 10Q�   Color$
 Gray��   	Direction$	 235Q�   Opacity$ .5Q�   RenderingBias$ Quality=�   ShadowDepth$ 5Q�._��� -    ��Name$ MyImage��$��1��$��0��$	��374��$	��290��$	��Top=�$��Right=�$��0,0,0,0q�$
��Fill=����� d� -    d�Name$ 	TglButton��$��2��$��0��$��Write Message��$��90��$��25��$
��Left=�$��57,0,0,0q�$y�Hidden=��� -    ��Name$ 
stackPanel��$��3��$��0��y���  �� ElementName$ 	TglButton�� �� Path$ 	IsChecked� �� 	Converter# [  .+��� -    ��Name$ MessageToHide��$��3��$��0��$	��300��$��75��$��Center=� �� TextWrapping$
 Wrap=�" �� VerticalScrollBarVisibility$ Visible=� �� AcceptsReturn . �� -	   $��3��$��0��$
��Hide��$��60��$��20��$��Center=�$��Bottom=�$��0,5,0,0q��� -
    ��Name$ 	DecodeBtn��$��1��$��1��$��50��$��25��$��Decode��$��Center=�$	��Top=�$��0,20,0,0q�$y�Hidden=��� $��1��$��1��$��Message:��$��20(�$��Buxton Sketch)�$��Center=�$	��Top=�$��20��$��0,60,0,0q�y��� $ 	DecodeBtn��$ 
Visibility��� $��1��$��1��$	��150��$	��210��$��Bottom=�$��0,0,0,10q�$��1q����	#FF000000y��� $ 	DecodeBtn��$ 
Visibility�._��� -   $ MessageText��$��1��$��1��$	��150��$	��210��$
 Wrap=�$��Bottom=�  �� 
IsReadOnly  . $ Visible=�$��15(��� -   ! ��Name$! prompt��$��4��$��0��" �� TextAlignment$" Center=�$��Stretch=�$��Stretch=�$
��Bold"�$
��Wrap=��� -   $ DecodeResult��$��3��$��1��$	��100��$	��100��$	��Top=�$��0,7,0,0q��� -   $ LoadMessageFromFile��$��2��$��0��$��Load Text from File��$��Right=�$	��120��$��25��$��0,0,59,0q�$y�Hidden=��� -   $ SaveFileAsTxtBtn��$��2��$��1��$	��150��$��25��$��Save message as Text File��$��Center=�$��Center=�$y�Hidden=�