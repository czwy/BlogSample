using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;


namespace BlindsShaderSample
{

public class BlindsShader : ShaderEffect
{
    public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(BlindsShader), 0);
    public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress", typeof(double), typeof(BlindsShader), new UIPropertyMetadata(((double)(30D)), PixelShaderConstantCallback(0)));
    public static readonly DependencyProperty NumberOfBlindsProperty = DependencyProperty.Register("NumberOfBlinds", typeof(double), typeof(BlindsShader), new UIPropertyMetadata(((double)(5D)), PixelShaderConstantCallback(1)));
    public static readonly DependencyProperty Texture2Property = ShaderEffect.RegisterPixelShaderSamplerProperty("Texture2", typeof(BlindsShader), 1);
    public BlindsShader()
    {
        PixelShader pixelShader = new PixelShader();
        pixelShader.UriSource = new Uri("/BlindsShaderSample;component/Shader/ShaderSource/BlindsShader.ps", UriKind.Relative);
        this.PixelShader = pixelShader;

        this.UpdateShaderValue(InputProperty);
        this.UpdateShaderValue(ProgressProperty);
        this.UpdateShaderValue(NumberOfBlindsProperty);
        this.UpdateShaderValue(Texture2Property);
    }
    public Brush Input
    {
        get
        {
            return ((Brush)(this.GetValue(InputProperty)));
        }
        set
        {
            this.SetValue(InputProperty, value);
        }
    }
    /// <summary>The amount(%) of the transition from first texture to the second texture. </summary>
    public double Progress
    {
        get
        {
            return ((double)(this.GetValue(ProgressProperty)));
        }
        set
        {
            this.SetValue(ProgressProperty, value);
        }
    }
    /// <summary>The number of Blinds strips </summary>
    public double NumberOfBlinds
    {
        get
        {
            return ((double)(this.GetValue(NumberOfBlindsProperty)));
        }
        set
        {
            this.SetValue(NumberOfBlindsProperty, value);
        }
    }
    public Brush Texture2
    {
        get
        {
            return ((Brush)(this.GetValue(Texture2Property)));
        }
        set
        {
            this.SetValue(Texture2Property, value);
        }
    }
}
}
