   M S B A M L   `   `   `  ����H  DSteganography, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:SteganographySteganography  5      H DMahApps.Metro, Version=1.1.3.5, Culture=neutral, PublicKeyToken=nullV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5   	       Steganography.MainWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1	converterclr-namespace:Steganography     5      PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   "   Steganography.WindowBehavior    HideCloseButton  .  � Title$ Steganography��5   	   $	��550��6   $	��625��6,    � 
ResizeMode$ NoResize=�5       � WindowStartupLocation$ CenterScreen=�6    � WindowStyle$ 
ToolWindow=�6>   (  "MahApps.Metro.Controls.MetroWindow   ShowCloseButton .  #��� ��5
         EnableDWMDropShadow .     NonActiveWindowTitleBrush# � ��5      
�������   WindowTitleBrush �	#FF1C9362$	��550��5      $	��550��6'   $	��625��67   $	��625��6F   	 � 	Resources	 5      +  %Steganography.BoolVisibilityConverter   BoolVisibilityConverter��%   &	          5   
   6,   5      .��5      � 
 � RowDefinitions
 5   
   �� 5      $��* �6   �� 5      $	��10* �6   �� 5      $��* �6   �� 5      $
��3.5* �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $��* �6   .+�5   
   o� $��0��6   $��2��6   $��0��60   $
��Auto��6@   
�������$��2q�6`   ��5      �� 5       .R�5!      ]� $��Stretch=�6    5"      .@�5$      m� $��Image��6   $��10,0,0,0q�6&   .@�5%      m� -   5        $
��Open��5%      6)   �� 5&      m� 5'      -   5        $
��Save��5'      6)   5(      m� 5)      -   5        $��About��5)      6&   m� 5*      -   5        $��Close��5*      $��Right=�67   $��0,0,80,2q�6S   
���  ��6e   5+      �� 5,   
   $��0��6   $��0��6!   $��2��61   $��Bottom=�6E   �� 5-   
   $��1��6   $��1��6!   
�������$��2q�6D   $��2��6X   $
��Left=�6c   $��10,-3,0,-173q�6~    � LayoutTransform 5.      �� 5/       �� Angle$ 90Q�6"   51      �� 52   
   $��4��6   $��1��6!   $��Łukasz Paczyński��61   $��Center=�6I   $��Center=�6d   $��20(�6�   $��Buxton Sketch)�6�   �� 53   
   $��1��6   $��0��6   $��1q�6.   ���	#FF000000$	��374��6Z   $	��290��6f   $	��Top=�6s   $��Right=�6�   $��	0,20,20,0q�6�   
������� J� Effect 54      3  -System.Windows.Media.Effects.DropShadowEffect  55         
BlurRadius$ 10Q�6#      Color$
 Gray��63      	Direction$	 235Q�6@      Opacity$ .5Q�6P      RenderingBias$ Quality=�6]      ShadowDepth$ 5Q�6u   ._�57      �� -   5         ��Name$ MyImage��57      $��1��6&   $��0��63   $	��374��6C   $	��290��6O   $	��Top=�6\   $��Right=�6t   $��0,0,0,0q�6�   $
��Fill=�6�   ��6�   �� 6�   58      d� 59   
   -   5         d�Name$ 	TglButton��59      $��2��6+   $��0��68   $��Write Message��6H   $��90��6a   $��25��6l   $
��Left=�6x   $��57,0,0,0q�6�   $y�Hidden=�6�   �� 5:   
   -   5         ��Name$ 
stackPanel��5:      $��3��6)   $��0��66   y�6F   ��  �� ElementName$ 	TglButton�� �� Path$ 	IsChecked� �� 	Converter# [  .+�5;      �� -   5         ��Name$ MessageToHide��5;      $��3��6-   $��0��6:   $	��300��6J   $��75��6V   $��Center=�6b    �� TextWrapping$
 Wrap=�6   " �� VerticalScrollBarVisibility$ Visible=�6�    �� AcceptsReturn . 6�   �� 5<      -	   5        $��3��5<      $��0��6"   $
��Hide��62   $��60��6A   $��20��6L   $��Center=�6X   $��Bottom=�6u   $��0,5,0,0q�6�   6�   5=      �� 5>   
   -
   5         ��Name$ 	DecodeBtn��5>      $��1��6$   $��1��61   $��50��6A   $��25��6L   $��Decode��6X   $��Center=�6i   $	��Top=�6�   $��0,20,0,0q�6�   $y�Hidden=�6�   6�   �� 5?   
   $��1��6   $��1��6!   $��Message:��61   $��20(�6A   $��Buxton Sketch)�6O   $��Center=�6j   $	��Top=�6�   $��20��6�   $��0,60,0,0q�6�   y�6�   �� $ 	DecodeBtn��$ 
Visibility��� 5@   
   $��1��6   $��1��6   $	��150��6.   $	��210��6:   $��Bottom=�6G   $��0,0,0,10q�6b   $��1q�6t   ���	#FF000000y�6�   �� $ 	DecodeBtn��$ 
Visibility�._�5A      �� -   5        $ MessageText��5A      $��1��6+   $��1��68   $	��150��6H   $	��210��6T   $
 Wrap=�6a   $��Bottom=�6u     �� 
IsReadOnly  . $ Visible=�6�   $��15(�6�   5B      �� 5C   
   -   5        ! ��Name$! prompt��5C      $��4��6$   $��0��61   " �� TextAlignment$" Center=�6A   $��Stretch=�6X   $��Stretch=�6v   $
��Bold"�6�   $
��Wrap=�6�   �� 5D   
   -   5        $ DecodeResult��5D      $��3��6&   $��1��63   $	��100��6C   $	��100��6O   $	��Top=�6\   $��0,7,0,0q�6t   �� 5E   
   -   5        $ LoadMessageFromFile��5E      $��2��6.   $��0��6;   $��Load Text from File��6K   $��Right=�6i   $	��120��6�   $��25��6�   $��0,0,59,0q�6�   $y�Hidden=�6�   6�   �� 5F   
   -   5        $ SaveFileAsTxtBtn��5F      $��2��6+   $��1��68   $	��150��6H   $��25��6T   $��Save message as Text File��6`   $��Center=�6�   $��Center=�6�   $y�Hidden=�6�   6�   5G      5H      