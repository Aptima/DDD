//
// This file was generated by the JavaTM Architecture for XML Binding(JAXB) Reference Implementation, v2.1-b02-fcs 
// See <a href="http://java.sun.com/xml/jaxb">http://java.sun.com/xml/jaxb</a> 
// Any modifications to this file will be lost upon recompilation of the source schema. 
// Generated on: 2009.05.25 at 05:05:58 PM EDT 
//

package com.aptima.ddd.networktools.jxab;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;
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
 *       &lt;sequence&gt;
 *         &lt;element ref=&quot;{}CustomAttribute&quot; maxOccurs=&quot;unbounded&quot;/&gt;
 *       &lt;/sequence&gt;
 *     &lt;/restriction&gt;
 *   &lt;/complexContent&gt;
 * &lt;/complexType&gt;
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "", propOrder = { "customAttribute" })
@XmlRootElement(name = "CustomAttributesType")
public class CustomAttributesType  implements Serializable{

	@XmlElement(name = "CustomAttribute", required = true)
	protected List<CustomAttribute> customAttribute;

	public CustomAttributesType() {
		super();
	}

	public CustomAttributesType(CustomAttribute... customAttributes) {
		super();
		for (CustomAttribute e : customAttributes)
			this.customAttribute.add(e);
	}

	public CustomAttributesType(List<CustomAttribute> customAttribute) {
		super();
		this.customAttribute = customAttribute;
	}

	/**
	 * Gets the value of the customAttribute property.
	 * 
	 * <p>
	 * This accessor method returns a reference to the live list, not a
	 * snapshot. Therefore any modification you make to the returned list will
	 * be present inside the JAXB object. This is why there is not a
	 * <CODE>set</CODE> method for the customAttribute property.
	 * 
	 * <p>
	 * For example, to add a new item, do as follows:
	 * 
	 * <pre>
	 * getCustomAttribute().add(newItem);
	 * </pre>
	 * 
	 * 
	 * <p>
	 * Objects of the following type(s) are allowed in the list
	 * {@link CustomAttribute }
	 * 
	 * 
	 */
	public List<CustomAttribute> getCustomAttribute() {
		if (customAttribute == null) {
			customAttribute = new ArrayList<CustomAttribute>();
		}
		return this.customAttribute;
	}

	public String toString() {
		String ret = "[";
		for( CustomAttribute a : this.getCustomAttribute()) {
			ret+=a.toString();
		}
		return ret+"]";
	}

}
