   M S B A M L   `   `   `  ����P  LPPH_153P_Configurator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:PPH_153P_Configurator.WindowsPPH_153P_Configurator.Windows  5   	   1    +PPH_153P_Configurator.Windows.ChannelChoose   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   =local+clr-namespace:PPH_153P_Configurator.Windows     5   	   $	��350��5   	   $	��300��6     � WindowStartupLocation$  CenterScreen=�5	   	    � 	Resources 5
         MouseOverBrush �� 
StartPoint �� EndPoint � Color � Offset �� 
TargetType �� Property �� Value	 �� Triggers
 P� Property P� Value �� 	Resources��%E  &	        '�� �     7   �� 5   
   $	 0,0&�65   $	 0,1&�6F   ��5      � � 5      $ #E4F0FD��6#   $ 0Q�63   � 5      $ #D7EAFD��6#   $ 1Q�63   5      5      �� 5   
   " ��6   .�5      ��  � ��$ Normal"�6+   	 5      P� 5      
 �@J�IsMouseOver . . �5      ��  � ��8   63   5      P� 5      
 �@��
IsSelected . . �5      ��  � ��
 �   �63   5       5      ���� 5      '��       " ��6   .�5       ��  �@��CornerRadius$ 5��65   5!      5"      5#      5$      .��5%      �  � RowDefinitions 5'   
   �� 5(      �� 5)      $
��2.8* �6   �� 5*       � ColumnDefinitions 5,   
   �� 5-      �� 5.      .+�50   
   �� $��0��51      $��0 0 0 0q�52      $��2��53        SettingName#���  54      $��Center=�55      $	��Top=�56      $��18(�57      �� 59   
   -   5         ��Name$ config��59      $��2��5:      $��30��5<       �� TextAlignment$ Center=�5=      $��18(�5>      $��0��5?      $
��auto��5@      $��Bottom=�5A      $��	10 0 10 5q�5B      ��5;      �� *
ConfigName+5B      �� 5D   
   -   5         ��Name$ 
ChanChoose��5D      $
��auto��5E      $
��auto��5F      $��1��5G       �� CornerRadius$ 5��5H      $��0��5I      $��2��5J      $��1q�5K      ���	#4CFFFFFF���#000$��5q�5N      ._�5P      �� -   5         ��Name$ 
ChannelLst��5Q       �� SelectionMode$ Single=�5R      $��18(�5S      5U      5V      �� 5W   
   -   5        $
��auto��5X      $
��auto��5Y      $��2��5Z      $��5 5 5 5q�5[      ���#fff$��18(�5]       5_      ��%^   '��       �� 5`      " ��6   .�5a      ��  �@��CornerRadius$ 5��65   5b      5c      .��5d      �� $
��Wrap=�5e       �� TextAlignment$ Center=�5f        Save#���  5g      5i      �� 5j   
   $
��auto��5k      $
��auto��5l      $��2��5m      $��1��5n      $��5 5 5 5q�5o      ���#fff$��18(�5q       �� IsCancel .  5t      ��%^   '��       �� 5u      " ��6   .�5v      ��  �@��CornerRadius$ 5��65   5w      5x      .��5y      �� $
��Wrap=�5z      $ Center=�5{        Decline#���  5|      5~      5      5�      