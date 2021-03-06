//
// This file was generated by the JavaTM Architecture for XML Binding(JAXB) Reference Implementation, v2.1-b02-fcs 
// See <a href="http://java.sun.com/xml/jaxb">http://java.sun.com/xml/jaxb</a> 
// Any modifications to this file will be lost upon recompilation of the source schema. 
// Generated on: 2009.05.25 at 05:05:58 PM EDT 
//

package com.aptima.ddd.networktools.jxab;

import java.io.Serializable;
import java.lang.reflect.Field;
import java.math.BigInteger;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlType;

/**
 * <p>
 * Java class for anonymous complex type.
 * 
 * <p>
 * The following schema fragment specifies the expected content contained within
 * this class.
 * 
 * <pre>
 * &lt;complexType&gt;
 *   &lt;complexContent&gt;
 *     &lt;restriction base=&quot;{http://www.w3.org/2001/XMLSchema}anyType&quot;&gt;
 *       &lt;choice&gt;
 *         &lt;element ref=&quot;{}AttributeCollectionType&quot;/&gt;
 *         &lt;element ref=&quot;{}CapabilityType&quot;/&gt;
 *         &lt;element ref=&quot;{}ConeType&quot;/&gt;
 *         &lt;element ref=&quot;{}CustomAttributesType&quot;/&gt;
 *         &lt;element ref=&quot;{}EmitterType&quot;/&gt;
 *         &lt;element ref=&quot;{}SensorArrayType&quot;/&gt;
 *         &lt;element ref=&quot;{}SensorType&quot;/&gt;
 *         &lt;element ref=&quot;{}VelocityType&quot;/&gt;
 *         &lt;element ref=&quot;{}PolygonType&quot;/&gt;
 *         &lt;element ref=&quot;{}StateTableType&quot;/&gt;
 *         &lt;element ref=&quot;{}StringListType&quot;/&gt;
 *         &lt;element ref=&quot;{}LocationType&quot;/&gt;
 *         &lt;element ref=&quot;{}VulnerabilityType&quot;/&gt;
 *         &lt;element ref=&quot;{}StringType&quot;/&gt;
 *         &lt;element ref=&quot;{}IntegerType&quot;/&gt;
 *         &lt;element ref=&quot;{}BooleanType&quot;/&gt;
 *         &lt;element ref=&quot;{}DoubleType&quot;/&gt;
 *       &lt;/choice&gt;
 *     &lt;/restriction&gt;
 *   &lt;/complexContent&gt;
 * &lt;/complexType&gt;
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "", propOrder = { "attributeCollectionType", "capabilityType",
		"coneType", "customAttributesType", "emitterType", "sensorArrayType",
		"sensorType", "velocityType", "polygonType", "stateTableType",
		"stringListType", "locationType", "vulnerabilityType", "stringType",
		"integerType", "booleanType", "doubleType" })
@XmlRootElement(name = "Value")
public class Value implements Serializable{

	@XmlElement(name = "AttributeCollectionType")
	protected AttributeCollectionType attributeCollectionType;
	@XmlElement(name = "BooleanType")
	protected Boolean booleanType;
	@XmlElement(name = "CapabilityType")
	protected CapabilityType capabilityType;
	@XmlElement(name = "ConeType")
	protected ConeType coneType;
	@XmlElement(name = "CustomAttributesType")
	protected CustomAttributesType customAttributesType;
	@XmlElement(name = "DoubleType")
	protected Double doubleType;
	@XmlElement(name = "EmitterType")
	protected EmitterType emitterType;
	@XmlElement(name = "IntegerType")
	protected BigInteger integerType;
	@XmlElement(name = "LocationType")
	protected LocationType locationType;
	@XmlElement(name = "PolygonType")
	protected PolygonType polygonType;
	@XmlElement(name = "SensorArrayType")
	protected SensorArrayType sensorArrayType;
	@XmlElement(name = "SensorType")
	protected SensorType sensorType;
	@XmlElement(name = "StateTableType")
	protected StateTableType stateTableType;
	@XmlElement(name = "StringListType")
	protected StringListType stringListType;
	@XmlElement(name = "StringType")
	protected String stringType;
	@XmlElement(name = "VelocityType")
	protected VelocityType velocityType;
	@XmlElement(name = "VulnerabilityType")
	protected Object vulnerabilityType;

	// Reflection Magic to get the non-null choice for this XML element
	// This code is NOT automatically generated by JXAB, but is here t make it
	// Easier to use.
	public Object get_Value() {
		for (Field field : Value.class.getDeclaredFields()) {
			try {
				Object f = field.get(this);
				if (!f.equals(null)) {
					return f;
				}
			} catch (Exception e) {
				// Meh- we tried
			}
		}

		this.stringType = "Unhandled";
		
		return this.getStringType();
	}

	/**
	 * Gets the value of the attributeCollectionType property.
	 * 
	 * @return possible object is {@link AttributeCollectionType }
	 * 
	 */
	public AttributeCollectionType getAttributeCollectionType() {
		return attributeCollectionType;
	}

	/**
	 * Gets the value of the capabilityType property.
	 * 
	 * @return possible object is {@link CapabilityType }
	 * 
	 */
	public CapabilityType getCapabilityType() {
		return capabilityType;
	}

	/**
	 * Gets the value of the coneType property.
	 * 
	 * @return possible object is {@link ConeType }
	 * 
	 */
	public ConeType getConeType() {
		return coneType;
	}

	/**
	 * Gets the value of the customAttributesType property.
	 * 
	 * @return possible object is {@link CustomAttributesType }
	 * 
	 */
	public CustomAttributesType getCustomAttributesType() {
		return customAttributesType;
	}

	/**
	 * Gets the value of the doubleType property.
	 * 
	 * @return possible object is {@link Double }
	 * 
	 */
	public Double getDoubleType() {
		return doubleType;
	}

	/**
	 * Gets the value of the emitterType property.
	 * 
	 * @return possible object is {@link EmitterType }
	 * 
	 */
	public EmitterType getEmitterType() {
		return emitterType;
	}

	/**
	 * Gets the value of the integerType property.
	 * 
	 * @return possible object is {@link BigInteger }
	 * 
	 */
	public BigInteger getIntegerType() {
		return integerType;
	}

	/**
	 * Gets the value of the locationType property.
	 * 
	 * @return possible object is {@link LocationType }
	 * 
	 */
	public LocationType getLocationType() {
		return locationType;
	}

	/**
	 * Gets the value of the polygonType property.
	 * 
	 * @return possible object is {@link PolygonType }
	 * 
	 */
	public PolygonType getPolygonType() {
		return polygonType;
	}

	/**
	 * Gets the value of the sensorArrayType property.
	 * 
	 * @return possible object is {@link SensorArrayType }
	 * 
	 */
	public SensorArrayType getSensorArrayType() {
		return sensorArrayType;
	}

	/**
	 * Gets the value of the sensorType property.
	 * 
	 * @return possible object is {@link SensorType }
	 * 
	 */
	public SensorType getSensorType() {
		return sensorType;
	}

	/**
	 * Gets the value of the stateTableType property.
	 * 
	 * @return possible object is {@link StateTableType }
	 * 
	 */
	public StateTableType getStateTableType() {
		return stateTableType;
	}

	/**
	 * Gets the value of the stringListType property.
	 * 
	 * @return possible object is {@link StringListType }
	 * 
	 */
	public StringListType getStringListType() {
		return stringListType;
	}

	/**
	 * Gets the value of the stringType property.
	 * 
	 * @return possible object is {@link String }
	 * 
	 */
	public String getStringType() {
		return stringType;
	}

	/**
	 * Gets the value of the velocityType property.
	 * 
	 * @return possible object is {@link VelocityType }
	 * 
	 */
	public VelocityType getVelocityType() {
		return velocityType;
	}

	/**
	 * Gets the value of the vulnerabilityType property.
	 * 
	 * @return possible object is {@link Object }
	 * 
	 */
	public Object getVulnerabilityType() {
		return vulnerabilityType;
	}

	/**
	 * Gets the value of the booleanType property.
	 * 
	 * @return possible object is {@link Boolean }
	 * 
	 */
	public Boolean isBooleanType() {
		return booleanType;
	}

	/**
	 * Sets the value of the attributeCollectionType property.
	 * 
	 * @param value
	 *            allowed object is {@link AttributeCollectionType }
	 * 
	 */
	public void setAttributeCollectionType(AttributeCollectionType value) {
		this.attributeCollectionType = value;
	}

	/**
	 * Sets the value of the booleanType property.
	 * 
	 * @param value
	 *            allowed object is {@link Boolean }
	 * 
	 */
	public void setBooleanType(Boolean value) {
		this.booleanType = value;
	}

	/**
	 * Sets the value of the capabilityType property.
	 * 
	 * @param value
	 *            allowed object is {@link CapabilityType }
	 * 
	 */
	public void setCapabilityType(CapabilityType value) {
		this.capabilityType = value;
	}

	/**
	 * Sets the value of the coneType property.
	 * 
	 * @param value
	 *            allowed object is {@link ConeType }
	 * 
	 */
	public void setConeType(ConeType value) {
		this.coneType = value;
	}

	/**
	 * Sets the value of the customAttributesType property.
	 * 
	 * @param value
	 *            allowed object is {@link CustomAttributesType }
	 * 
	 */
	public void setCustomAttributesType(CustomAttributesType value) {
		this.customAttributesType = value;
	}

	/**
	 * Sets the value of the doubleType property.
	 * 
	 * @param value
	 *            allowed object is {@link Double }
	 * 
	 */
	public void setDoubleType(Double value) {
		this.doubleType = value;
	}

	/**
	 * Sets the value of the emitterType property.
	 * 
	 * @param value
	 *            allowed object is {@link EmitterType }
	 * 
	 */
	public void setEmitterType(EmitterType value) {
		this.emitterType = value;
	}

	/**
	 * Sets the value of the integerType property.
	 * 
	 * @param value
	 *            allowed object is {@link BigInteger }
	 * 
	 */
	public void setIntegerType(BigInteger value) {
		this.integerType = value;
	}

	/**
	 * Sets the value of the locationType property.
	 * 
	 * @param value
	 *            allowed object is {@link LocationType }
	 * 
	 */
	public void setLocationType(LocationType value) {
		this.locationType = value;
	}

	/**
	 * Sets the value of the polygonType property.
	 * 
	 * @param value
	 *            allowed object is {@link PolygonType }
	 * 
	 */
	public void setPolygonType(PolygonType value) {
		this.polygonType = value;
	}

	/**
	 * Sets the value of the sensorArrayType property.
	 * 
	 * @param value
	 *            allowed object is {@link SensorArrayType }
	 * 
	 */
	public void setSensorArrayType(SensorArrayType value) {
		this.sensorArrayType = value;
	}

	/**
	 * Sets the value of the sensorType property.
	 * 
	 * @param value
	 *            allowed object is {@link SensorType }
	 * 
	 */
	public void setSensorType(SensorType value) {
		this.sensorType = value;
	}

	/**
	 * Sets the value of the stateTableType property.
	 * 
	 * @param value
	 *            allowed object is {@link StateTableType }
	 * 
	 */
	public void setStateTableType(StateTableType value) {
		this.stateTableType = value;
	}

	/**
	 * Sets the value of the stringListType property.
	 * 
	 * @param value
	 *            allowed object is {@link StringListType }
	 * 
	 */
	public void setStringListType(StringListType value) {
		this.stringListType = value;
	}

	/**
	 * Sets the value of the stringType property.
	 * 
	 * @param value
	 *            allowed object is {@link String }
	 * 
	 */
	public void setStringType(String value) {
		this.stringType = value;
	}

	/**
	 * Sets the value of the velocityType property.
	 * 
	 * @param value
	 *            allowed object is {@link VelocityType }
	 * 
	 */
	public void setVelocityType(VelocityType value) {
		this.velocityType = value;
	}

	/**
	 * Sets the value of the vulnerabilityType property.
	 * 
	 * @param value
	 *            allowed object is {@link Object }
	 * 
	 */
	public void setVulnerabilityType(Object value) {
		this.vulnerabilityType = value;
	}

	// public Value new_Value(Object value) {
	// Value v = new Value();
	// for (Field field : Value.class.getDeclaredFields()) {
	//
	// if (value.getClass().equals(field.getType())) {
	//
	// try {
	// field.set(v, value);
	// } catch (IllegalArgumentException e) {
	// // TODO Auto-generated catch block
	// e.printStackTrace();
	// } catch (IllegalAccessException e) {
	// // TODO Auto-generated catch block
	// e.printStackTrace();
	// }
	//
	// }
	// }
	// return v;
	// }

	public String toString() {
		return get_Value().toString();
	}

}
